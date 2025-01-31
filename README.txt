# Temat zadania
Komunikator internetowy typu GG

### Opis protokołu komunikacyjnego
W naszym projekcie użyliśmy protokół TCP do komunikacji między serwerem, a klientem. Protokuł ten jest połączeniowy co wymaga nawiązania i zakończenia sesji. Dodatkowo jest on niezawodny poprezz retransmisje pakietów które nie dotarły do celu/

### Opis implementacji, w tym krótki opis zawartości plików źródłowych
#### Implementacja serwera
Podczas uruchomienia serwera otwierana jest zewnętrzna baza danych (w przypadku, gdy baza nie istenije jest ona tworzona). Baza służy za miejsce przechowywania wiadomość wysłanych do użytkowników. Następnie serwer nasłuchuje na połączenie przychodzące od klienta na wskazanym przez użytkownika porcie.\
Wiadomość ma format 1:UserNameSource:UserNameDestination:Message, gdzie:
1 - odpowiada formacie wiadomości, UserNameSource - nazwę użytkownika który nadał wiadomość, UserNameDestination - nazwę użytkownika do którego ma trafić wiadomość, Message - przesyłana wiadomość.\
*Przykład: 1:Klient1:Klient2:Hej*.\
W przypadku gdy użytkownik jest online, wiadomości są wysyłane do niego bezpośrednio.\
W przypadku gdy użytkownik jest offline wiadomości są zapisywane do bazy.\
Serwer jest współbieżny poprzez wykorzystanie wielowątkowości (jedne klient = jeden wątek)
#### Implementacja klienta
???
### Sposób komplilacji, uruchomienia i obsługi programów projektu
#### Kompilacja serwera na Linuxie
g++ -Wall ServerLinux.cpp -lsqlite3 -o [nazwa_wyjściowa]\
*Aby serwer poprawnie się skompilował potrzebna jest biblioteka sqlite3 (sudo apt install sqlite3 dla Ubuntu) oraz argument -lsqlite3 w polecenu kompilującym.*
#### Obsługa klienta
Po uruchomieniu aplikacji klienta, użytkownik musi podać nazwę użytkownmika po której chce być rozpoznawalny (tzn. nick). Po zalogowaniu uzytkownik widzi ekran z wiadomościami. Z Lewej strony posiada listę użytkowników z którymi dotychczas rozmawiał. W celu rozpoczęcia rozmowy z nowym użytkownikiem należy kliknąć w opcje "Użytkownicy", następnie podać nick użytkownika do którego chcemy napisać i kliknąć przyciś odpowiadający za dodanie go. W celu powrócenia do ekrany z wiadomościami, należy kliknąć przycisk "Home". Uzytkonwik może zmienić konto poprzez przelogowanie się (klikajć przycisk "Wyloguj").\
Wysyłanie wiadomości odbywa się poprzez kliknięcie "Enter" lub ikonki dymku. 