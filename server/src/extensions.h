#pragma once
#include "basic_includes.h"
#include "client.h"

std::vector<std::string> split_string(const std::string& str, char delim);
std::string get_addr_ip(Client* client);