#include <thread>
#include <mutex>
#include <iostream>
#include <vector>
#include <map>
#include <cstring>
#include <algorithm>
#include <string>
#include <netinet/in.h>
#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <sqlite3.h>

#define PORT 8080
#define BUFFER_SIZE 1024

std::vector<class Client*> clients;
std::mutex clients_mutex;
sqlite3* db; // Globalna baza danych

#pragma region HelpFunctions
int numberOfCharsInArray(const std::string& str) {
    return str.length();
}

static int callback(void* NotUsed, int argc, char** argv, char** azColName) {
    for (int i = 0; i < argc; i++) {
        std::cout << (argv[i] ? argv[i] : "NULL") << "\t";
    }
    std::cout << std::endl;
    return 0;
}

std::vector<std::string> split_string(const std::string& str, char delim = ':') {
    std::vector<std::string> tokens;
    std::string token;

    for (size_t i = 0; i < str.length(); i++) {
        if (str[i] != delim) {
            token.push_back(str[i]);
        }
        else {
            tokens.push_back(token);
            token.clear();
        }
    }
    if (!token.empty()) {
        tokens.push_back(token);
    }
    return tokens;
}
#pragma endregion

class Packet {
public:
    int packetId;
    std::string packetBuffer;

    Packet(int id, const std::string& buffer) : packetId(id), packetBuffer(buffer) {}
};

class Client {
public:
    int cfd; // client socket
    bool isOnline = true;
    std::string username; // username
    struct sockaddr_in caddr; // client address info

    Client(int fd, const sockaddr_in& addr) : cfd(fd), caddr(addr) {}

    ~Client() {
        close(cfd);
    }
};

void initializeDatabase() {
    const char* createTableSQL = "CREATE TABLE IF NOT EXISTS komunikacja ("
        "wiadomosc TEXT NOT NULL"
        ");";

    char* errMessage = nullptr;
    int rc = sqlite3_exec(db, createTableSQL, nullptr, 0, &errMessage);
    if (rc != SQLITE_OK) {
        std::cerr << "SQL error: " << errMessage << std::endl;
        sqlite3_free(errMessage);
    }
    else {
        std::cout << "Table initialized successfully!\n";
    }
}

void saveMessageToDatabase(const std::string& sender, const std::string& receiver, const std::string& message) {
    char* errMessage = nullptr;
    std::string finalMessage = "1:" + sender + ":" + receiver + ":" + message;

    std::string sql = "INSERT INTO komunikacja (wiadomosc) VALUES ('"+ finalMessage +"');";

    int rc = sqlite3_exec(db, sql.c_str(), nullptr, 0, &errMessage);
    if (rc != SQLITE_OK) {
        std::cerr << "Failed to insert message: " << errMessage << std::endl;
        sqlite3_free(errMessage);
    }
    else {
        std::cout << "Message saved to database.\n";
    }
}

void sendMessage(const std::string& message) {
    std::vector<std::string> splitted = split_string(message, ':');
    if (splitted.size() < 3) {
        std::cerr << "Invalid message format: " << message << std::endl;
        return;
    }

    std::string sender = splitted[1];
    std::string receiver = splitted[2];
    std::string msgContent = splitted[3];


    std::lock_guard<std::mutex> lock(clients_mutex);
    for (Client* client : clients) {
        if (receiver == client->username) {
            if (client->isOnline) {
                write(client->cfd, message.c_str(), message.size());
                printf("Message sent to user %s\n", client->username.c_str());
                return;
            }
        }
    }
    saveMessageToDatabase(sender, receiver, msgContent);
    return;
}

void handle_client(void* arg) {
    std::string buffer;

    Client* c = (Client*)arg;

    printf("Client %s connected\n", inet_ntoa((struct in_addr)c->caddr.sin_addr));

    printf("Hello %s\n", c->username.c_str());

    while (true) {
        buffer.clear();
        char tempBuffer[BUFFER_SIZE];
        memset(tempBuffer, 0, BUFFER_SIZE);
        int bytes_received = recv(c->cfd, tempBuffer, BUFFER_SIZE, 0);

        if (bytes_received <= 0) {
            printf("%s has disconnected!\n", c->username.c_str());
            break;
        }
        else {
            buffer = tempBuffer;
            std::string packetId = split_string(buffer, ':')[0];
            Packet* p = new Packet(std::stoi(packetId), buffer);
            sendMessage(p->packetBuffer);
            printf("%s sent: %s\n", c->username.c_str(), p->packetBuffer.c_str());
            delete p;
        }
    }

    std::lock_guard<std::mutex> lock(clients_mutex);
    for (size_t i = 0; i < clients.size(); i++) {
        Client* client = clients[i];
        if (client->cfd == c->cfd) {
            clients.erase(clients.begin() + i);
            delete client;
            break;
        }
    }
}

void sendStoredMessages(Client* client) {
    std::string sql = "SELECT wiadomosc FROM komunikacja WHERE wiadomosc LIKE '1:%:" + client->username + ":%';";
    sqlite3_stmt* stmt;

    if (sqlite3_prepare_v2(db, sql.c_str(), -1, &stmt, nullptr) != SQLITE_OK) {
        std::cerr << "SQL error: " << sqlite3_errmsg(db) << std::endl;
        return;
    }

    while (sqlite3_step(stmt) == SQLITE_ROW) {
        std::string message = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
        write(client->cfd, message.c_str(), message.size());
    }

    sqlite3_finalize(stmt);

    sql = "DELETE FROM komunikacja WHERE wiadomosc LIKE '%:" + client->username + ":%';";
    char* errMessage = nullptr;
    if (sqlite3_exec(db, sql.c_str(), nullptr, 0, &errMessage) != SQLITE_OK) {
        std::cerr << "Failed to delete messages: " << errMessage << std::endl;
        sqlite3_free(errMessage);
    }
}

void Connect(int clientSocket, sockaddr_in clientAddress, std::string username) {
    std::lock_guard<std::mutex> lock(clients_mutex);
    for (Client* client : clients) {
        if (client->username == username) {
            client->isOnline = true;
            std::thread(handle_client, client).detach();
            sendStoredMessages(client);
            return;
        }
    }

    Client* c = new Client(clientSocket, clientAddress);
    c->username = username;
    clients.push_back(c);

    sendStoredMessages(c);
    std::thread(handle_client, c).detach();
}
int main() {
    int server_socket, client_socket;
    struct sockaddr_in server_address, client_address;
    socklen_t client_address_len = sizeof(client_address);
    char tempBuffer[255];

    sqlite3_stmt* stmt;
    const char* sql = "SELECT name FROM sqlite_master WHERE type='table';";  // Zapytanie SQL

    int rc = sqlite3_open("SK2.db", &db);
    if (rc) {
        std::cerr << "Can't open database: " << sqlite3_errmsg(db) << std::endl;
        return 0;
    }
    else {
        std::cout << "Opened database successfully!\n";
    }

    initializeDatabase();

    // Przygotowanie zapytania
    if (sqlite3_prepare_v2(db, sql, -1, &stmt, 0) != SQLITE_OK) {
        std::cerr << "Błąd przy przygotowaniu zapytania: " << sqlite3_errmsg(db) << std::endl;
        sqlite3_close(db);
        return 1;
    }

    // Wykonanie zapytania i wypisanie wyników
    std::cout << "Tabele w bazie danych:\n";
    while (sqlite3_step(stmt) == SQLITE_ROW) {
        const unsigned char* table_name = sqlite3_column_text(stmt, 0);
        std::cout << table_name << std::endl;
    }



    server_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket == -1) {
        printf("Failed to create socket.\n");
        return -1;
    }

    int on = 1;
    setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));

    server_address.sin_family = AF_INET;
    server_address.sin_addr.s_addr = INADDR_ANY;
    server_address.sin_port = htons(PORT);

    if (bind(server_socket, (struct sockaddr*)&server_address, sizeof(server_address)) == -1) {
        printf("Binding failed.\n");
        close(server_socket);
        return -1;
    }

    if (listen(server_socket, 10) == -1) {
        printf("Listening failed.\n");
        close(server_socket);
        return -1;
    }

    printf("Server is running on port %d\n", PORT);

    while (true) {
        client_socket = accept(server_socket, (struct sockaddr*)&client_address, &client_address_len);
        if (client_socket == -1) {
            printf("Failed to accept client connection.\n");
            continue;
        }

        memset(tempBuffer, 0, 255);
        recv(client_socket, tempBuffer, 255, 0);
        std::string tmpBuff = tempBuffer;
        tmpBuff.erase(std::remove(tmpBuff.begin(), tmpBuff.end(), '\n'), tmpBuff.cend());

        Connect(client_socket, client_address, tmpBuff);
    }

    sqlite3_finalize(stmt);
    sqlite3_close(db);
    close(server_socket);
    return 0;
}