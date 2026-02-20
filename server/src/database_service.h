#pragma once

#include "database_service.h"
#include "sqlite3.h"
#include "basic_includes.h"
#include "client.h"

class DatabaseService {
private:
	bool openConnection();
public:
	sqlite3* db;

	DatabaseService();
	~DatabaseService();
	
	bool init();
	bool addMessage(const std::string& sender, const std::string& receiver, const std::string& message);
	std::vector<std::string> getHisotryMessage(const std::string& client);
};
