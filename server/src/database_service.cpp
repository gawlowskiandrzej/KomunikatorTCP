#include "database_service.h"
#include "sqlite3.h"
#include <iostream>

class DatabaseService {
public:
	void init() {
        const char* createTableSQL = "CREATE TABLE IF NOT EXISTS komunikacja ("
            "wiadomosc TEXT NOT NULL"
            ");";

        char* errMessage = nullptr;
        int rc = sqlite3_exec(db, createTableSQL, nullptr, 0, &errMessage);
        if (rc != SQLITE_OK) {
            std::cerr << "SQL error: " << errMessage << std::endl;
            sqlite3_free(errMessage);
        }
        else {
            std::cout << "Table initialized successfully!\n";
        }

	}

};