#include "extensions.h"

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
std::string get_addr_ip(Client* client)
{
    char ipstr[INET_ADDRSTRLEN] = { 0 };

    const char* result = inet_ntop(
        AF_INET,
        &client->addres.sin_addr,
        ipstr,
        sizeof(ipstr)
    );

    if (!result)
    {
        perror("inet_ntop failed");
        return "INVALID_IP";
    }

    return std::string(ipstr);
}