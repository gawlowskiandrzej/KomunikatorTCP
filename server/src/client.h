#pragma once
#include "basic_includes.h"
#include "platform_sockets.h"

class Client {
public:
    Client(socket_t client_socket, const std::string& username, sockaddr_in& addr)
    {
        cfd = client_socket;
        this->username = username;
        this->addres = addr;
    }
    socket_t cfd; 
    bool isOnline = true;
    std::string username;
    sockaddr_in addres;
};
