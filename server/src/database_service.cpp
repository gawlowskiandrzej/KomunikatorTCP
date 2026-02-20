#include "database_service.h"

DatabaseService::DatabaseService()
{
	if (openConnection())
		init();
}

DatabaseService::~DatabaseService()
{
	if (db)
		sqlite3_close(db);
}

bool DatabaseService::openConnection()
{
	int rc = sqlite3_open("SK2.db", &db);
	if (rc) {
		std::cerr << "Can't open database: " << sqlite3_errmsg(db) << std::endl;
		return false;
	}
	else {
		std::cout << "Opened database successfully!\n";
		return true;
	}
}

bool DatabaseService::init()
{
	const char* createTableSQL = "CREATE TABLE IF NOT EXISTS komunikacja ("
		"wiadomosc TEXT NOT NULL"
		");";

	char* errMessage = nullptr;
	int rc = sqlite3_exec(db, createTableSQL, nullptr, 0, &errMessage);

	if (rc != SQLITE_OK) {

		std::cerr << "SQL error: " << errMessage << std::endl;
		sqlite3_free(errMessage);

		return false;
	}
	else {
		std::cout << "Table initialized successfully!\n";

		return true;
	}

}
bool DatabaseService::addMessage(const std::string& sender, const std::string& receiver, const std::string& message)
{
	char* errMessage = nullptr;
	std::string finalMessage = "1:" + sender + ":" + receiver + ":" + message;

	std::string sql = "INSERT INTO komunikacja (wiadomosc) VALUES ('" + finalMessage + "');";

	int rc = sqlite3_exec(db, sql.c_str(), nullptr, 0, &errMessage);
	if (rc != SQLITE_OK) {
		std::cerr << "Failed to insert message: " << errMessage << std::endl;
		sqlite3_free(errMessage);
		return false;
	}
	else {
		std::cout << "Message saved to database.\n";
		return true;
	}
}
std::vector<std::string> DatabaseService::getHisotryMessage(const std::string& clientUsername)
{
	std::vector<std::string> messages{};

	std::string sql = "SELECT wiadomosc FROM komunikacja WHERE wiadomosc LIKE '1:%:" + clientUsername + ":%' or wiadomosc LIKE '1:" + clientUsername + ":%:%';";

	sqlite3_stmt* stmt;

	if (sqlite3_prepare_v2(db, sql.c_str(), -1, &stmt, nullptr) != SQLITE_OK) {
		std::cerr << "SQL error: " << sqlite3_errmsg(db) << std::endl;
		return {};
	}

	while (sqlite3_step(stmt) == SQLITE_ROW) {
		std::string message = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
		messages.push_back(message);
		std::this_thread::sleep_for(std::chrono::milliseconds(50));
	}

	return messages;
}
