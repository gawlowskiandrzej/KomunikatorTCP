#pragma once
#include "basic_includes.h"
#include "platform_sockets.h"
#include "database_service.h"
#include <mutex>
#include "client.h"

class Server {
public:
    Server(const int port);
    ~Server();

    void run();
    const int packet_delay = 100;
    void SendMessageToUser(const int dscOwner, const std::string& msg, const bool save = false);
private:
    socket_t server_socket;
    int port;
    std::vector<Client*> clients{};
    std::mutex clients_mutex{};
    DatabaseService* dbService;

    void HandleClientAction(Client* client);
    void ConnectClient(Client* client);
    void SendHistoryMessages(const Client* client);
    void init();
};
