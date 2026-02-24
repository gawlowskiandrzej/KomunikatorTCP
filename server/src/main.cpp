#include "server.h"
#include "platform_sockets.h"
#define _WIN32_WINNT 0x0500

#include <windows.h>

int main()
{

	PlatformServer platform;
	if (!platform.initialize()) return -1;
	Server* serv = new Server(8080);

	serv->run();

	platform.cleanup();
}
