#include "server.h"
#include "extensions.h"

Server::Server(const int port) : port(port), server_socket(-1) 
{
    this->init();
    this->dbService = new DatabaseService();
}

Server::~Server()
{
    if (server_socket != -1) CLOSE_SOCKET(server_socket);
}

void Server::run()
{
    //Run loop

    while (true) {
        sockaddr_in client_addr{};
        socklen_t addr_len = sizeof(client_addr);
        socket_t client_socket = accept(server_socket, (struct sockaddr*)&client_addr, &addr_len);
        if (client_socket == -1) {
            printf("Failed to accept client connection.\n");
            return;
        }

        char tempBuffer[255];
        memset(tempBuffer, 0, 255);

        SOCKET_READ(client_socket, tempBuffer, 255);
        std::string tmpBuff = tempBuffer;
        tmpBuff.erase(std::remove(tmpBuff.begin(), tmpBuff.end(), '\n'), tmpBuff.cend());
        Client* newClient = new Client(client_socket, tmpBuff, client_addr);

        Server::ConnectClient(newClient);
    }
}

void Server::SendMessageToUser(const int dscOwner, const std::string& msg, const bool save)
{
    std::vector<std::string> splitted = split_string(msg, ':');
    if (splitted.size() < 3) {
        std::cerr << "Invalid message format: " << msg << std::endl;
        return;
    }

    std::string sender = splitted[1];
    std::string receiver = splitted[2];
    std::string msgContent = splitted[3];


    std::lock_guard<std::mutex> lock(clients_mutex);
    for (Client* client : clients) {
        if (receiver == client->username) {
            if (client->isOnline) {
                SOCKET_WRITE(client->cfd, msg.c_str(), msg.size());
                printf("%s sent to user %s\n", sender.c_str(), client->username.c_str());
                break;
            }
        }
    }

    if (save)
        dbService->addMessage(sender, receiver, msgContent);
}

void Server::HandleClientAction(Client* client)
{
    //printf("Client %s connected\n", get_addr_ip(client));
    printf("Hello %s\n", client->username.c_str());
    char buffer[1024];
    while (true) {
        memset(buffer, 0, sizeof(buffer));
        int bytes = SOCKET_READ(client->cfd, buffer, sizeof(buffer));
        if (bytes <= 0) { printf("%s has disconnected!\n", client->username.c_str()); break; }

        std::string msg(buffer, bytes);
        
        Server::SendMessageToUser(client->cfd,msg, true);

        std::lock_guard<std::mutex> lock(clients_mutex);
        for (Client* client : clients) {
            if (client->cfd != client->cfd)
                SOCKET_WRITE(client->cfd, msg.c_str(), msg.size());
        }
    }

    std::lock_guard<std::mutex> lock(clients_mutex);
    clients.erase(std::remove(clients.begin(), clients.end(), client), clients.end());
    CLOSE_SOCKET(client->cfd);
    std::cout << "Client disconnected.\n";
}

void Server::ConnectClient(Client* new_client)
{
    std::lock_guard<std::mutex> lock(clients_mutex);
    for (Client* client : clients) {
        if (client->username == new_client->username) {
            client->isOnline = true;
            std::thread(&Server::HandleClientAction,this ,client).detach();
            Server::SendHistoryMessages(client);
            return;
        }
    }

    clients.push_back(new_client);

    Server::SendHistoryMessages(new_client);
    std::thread(&Server::HandleClientAction, this,new_client).detach();
}

void Server::SendHistoryMessages(const Client* client)
{
    auto storedMessages = dbService->getHisotryMessage(client->username);
    for (auto& message : storedMessages)
        SOCKET_WRITE(client->cfd, message.c_str(), message.size());

    // Terminator for end of loading messages
    std::string msg = "1::" + client->username + ":";
    SOCKET_WRITE(client->cfd, msg.c_str(), msg.size());
}

void Server::init()
{
    // Init sockets

    server_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket < 0) { std::cerr << "Socket failed\n"; return; }

    int opt = 1;
    setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, (char*)&opt, sizeof(opt));

    sockaddr_in server_addr{};
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(port);

    if (bind(server_socket, (sockaddr*)&server_addr, sizeof(server_addr)) < 0) {
        std::cerr << "Bind failed\n"; return;
    }

    if (listen(server_socket, 10) < 0) {
        std::cerr << "Listen failed\n"; return;
    }

    std::cout << "Server listening on port " << port << "\n";
}
