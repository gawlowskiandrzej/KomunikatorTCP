#include <iostream>
#include <thread>
#include <mutex>
#include <vector>
#include <map>
#include <cstring>
#include <netinet/in.h>
#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#define PORT 8080
#define BUFFER_SIZE 1024


#pragma region HelpFunctions
int numberOfCharsInArray(char* array) {
    int numberOfChars = 0;
    while (*array != '\0' || *array != '\n') {
        numberOfChars++; array++;
    }
    return numberOfChars;
}
std::vector<std::string> split_string(const std::string& str, char delim = ' ') {
    std::vector<std::string> tokens;
    std::string token;

    for (size_t i = 0; i < str.length(); i++)
    {
        if (str[i] != delim)
        {
            token.push_back(str[i]);
        }
        else { tokens.push_back(token); token.clear(); }
    }
    return tokens;
}
#pragma endregion

struct packetStruct {
    int packetId;
    char packetBuffer[BUFFER_SIZE];
};

struct clientStruct {
    int cfd; // client socket
    char username[255]; // username
    struct sockaddr_in caddr; // client addres info
};

void translatePacket(packetStruct* packet)
{
    // Translating packets to call functions

    int packetId = packet->packetId;

    switch (packetId)
    {
        case 1: { std::vector<std::string>splitted = split_string(packet->packetBuffer, ':'); sendMessage(splitted[0], splitted[1], splitted[2]); break;}
        default:
            break;
    }
}

void sendMessage(std::string usernameSRC, std::string usernameDST, std::string message)
{

}

void handle_client(void* arg)
{
    char buffer[BUFFER_SIZE];

    struct clientStruct* c = (struct clientStruct*)arg;

    printf("client %s connected \n", inet_ntoa((struct in_addr)c->caddr.sin_addr));
    memset(c->username, 0, 255);
    // Getting username
    recv(c->cfd, c->username,255, 0);

    while (true)
    {
        memset(buffer, 0, BUFFER_SIZE);
        int bytes_received = recv(c->cfd, buffer, BUFFER_SIZE, 0);

        if (bytes_received <= 0)
        {
            printf("client has disconected!");
            break;
        }
        else 
        {
            printf("client %s send %d bytes -> %s", c->username, bytes_received, buffer);
        }
       
    }

    close(c->cfd);
}

int main() {
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
        struct clientStruct* c = (struct clientStruct*)malloc(sizeof(struct clientStruct));

        c->cfd = accept(server_socket, (struct sockaddr*)&c->caddr, &client_address_len);
        if (c->cfd == -1) {
            printf("Failed to accept client connection.\n");
            continue;
        }
        if (c->cfd > -1)
        {
            std::thread(handle_client, c).detach();
        }
    }

    close(server_socket);
    return 0;
}
