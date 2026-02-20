#pragma once
#include "basic_includes.h"

#ifdef _WIN32
#include <winsock2.h>
#include <ws2tcpip.h>
#pragma comment(lib, "ws2_32.lib")
#define CLOSE_SOCKET(s) closesocket(s)
#define SOCKET_READ(s, buf, len) recv(s, buf, len, 0)
#define SOCKET_WRITE(s, buf, len) send(s, buf, len, 0)
using socket_t = SOCKET;
#else
#include <unistd.h>
#include <arpa/inet.h>
#include <sys/socket.h>
#define CLOSE_SOCKET(s) close(s)
#define SOCKET_READ(s, buf, len) read(s, buf, len)
#define SOCKET_WRITE(s, buf, len) write(s, buf, len)
using socket_t = int;
#endif

class PlatformServer {
public:
#ifdef _WIN32
    bool initialize();
    void cleanup();
#else
    bool initialize();
    void cleanup();
#endif
};
