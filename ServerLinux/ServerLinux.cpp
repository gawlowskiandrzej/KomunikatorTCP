#include <iostream>
#include <thread>
#include <mutex>
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

#define PORT 8080
#define BUFFER_SIZE 1024

std::vector<class Client*> clients;
std::vector<std::string> messages;
std::mutex clients_mutex;

#pragma region HelpFunctions
int numberOfCharsInArray(const std::string& str) {
    return str.length();
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

void sendMessage(const std::string& message) {
    
    std::vector<std::string> splitted = split_string(message, ':');

    std::lock_guard<std::mutex> lock(clients_mutex);
    for (size_t i = 0; i < clients.size(); i++) {
        Client* client = clients[i];
        if (splitted[2] == client->username) {
            if (client->isOnline)
            {
                write(client->cfd, message.c_str(), message.size());
            }
            else
            {
                messages.push_back(message);
            }
            return;
        }
    }
}

void handle_client(void* arg) {
    std::string buffer;

    Client* c = (Client*)arg;

    printf("client %s connected \n", inet_ntoa((struct in_addr)c->caddr.sin_addr));


    while (true) {
        buffer.clear();
        char tempBuffer[BUFFER_SIZE];
        memset(tempBuffer, 0, BUFFER_SIZE);
        int bytes_received = recv(c->cfd, tempBuffer, BUFFER_SIZE, 0);

        if (bytes_received <= 0) {
            printf("client has disconnected!\n");
            break;
        }
        else {
            buffer = tempBuffer;
            std::string packetId = split_string(buffer, ':')[0];
            Packet* p = new Packet(std::stoi(packetId), buffer);
            sendMessage(p->packetBuffer);
            delete p;
        }
    }

    std::lock_guard<std::mutex> lock(clients_mutex);
    {
        for (size_t i = 0; i < clients.size(); i++) {
            Client* client = clients[i];
            if (client->cfd == c->cfd) {
                clients.erase(clients.begin() + i);
                delete client;
                break;
            }
        }
    }
}

void Connect(int clientSocket, sockaddr_in clientAddress, std::string username)
{
    std::lock_guard<std::mutex> lock(clients_mutex);
    for (int i = 0; i < clients.size(); i++)
    {
        if (clients[i]->username == username)
        {
            clients[i]->isOnline = true;
            std::thread(handle_client, clients[i]).detach();
            return;
        }
    }
    Client* c = new Client(clientSocket, clientAddress);
    c->username = username;
    clients.push_back(c);
    std::thread(handle_client, c).detach();
}
int main() {
    char tempBuffer[255];
    int server_socket, client_socket, on = 1;
    struct sockaddr_in server_address, client_address;
    socklen_t client_address_len = sizeof(client_address);

    server_socket = socket(AF_INET, SOCK_STREAM, 0);
    setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, (char*)&on, sizeof(on));
    if (server_socket == -1) {
        printf("Failed to create socket.\n");
        return -1;
    }

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

    printf("Server is running on port %d \n", PORT);

    while (true) {
        sockaddr_in client_addr;
        client_socket = accept(server_socket, (struct sockaddr*)&client_addr, &client_address_len);
        if (client_socket == -1) {
            printf("Failed to accept client connection.\n");
            continue;
        }

        memset(tempBuffer, 0, 255);
        recv(client_socket, tempBuffer, 255, 0);
        std::string tmpBuff = tempBuffer;
        tmpBuff.erase(std::remove(tmpBuff.begin(), tmpBuff.end(), '\n'), tmpBuff.cend());

        Connect(client_socket,client_addr,tmpBuff);
    }

    close(server_socket);
    return 0;
}
