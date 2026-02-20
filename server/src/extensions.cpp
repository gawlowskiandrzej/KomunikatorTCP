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