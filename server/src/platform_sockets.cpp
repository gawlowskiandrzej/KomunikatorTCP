#include "platform_sockets.h"
#include <iostream>

#ifdef _WIN32
bool PlatformServer::initialize() {
    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "WSAStartup failed\n";
        return false;
    }
    return true;
}

void PlatformServer::cleanup() {
    WSACleanup();
}
#endif
