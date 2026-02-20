#pragma once

#include "basic_includes.h"

class Packet {
public:
    int packetId;
    std::string packetBuffer;

    Packet(int id, const std::string& buffer) : packetId(id), packetBuffer(buffer) {}
};