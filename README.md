# Komunikator internetowy typu GG

## Opis protokołu komunikacyjnego
W naszym projekcie wykorzystaliśmy protokół **TCP** do komunikacji między serwerem a klientem. Jest to protokół połączeniowy, co oznacza, że wymaga nawiązania i zakończenia sesji. Dodatkowo, zapewnia niezawodność poprzez retransmisję pakietów, które nie dotarły do celu.

## Opis implementacji
### Implementacja serwera
Podczas uruchomienia serwera otwierana jest **zewnętrzna baza danych**. Jeśli baza nie istnieje, zostaje utworzona. Służy ona do przechowywania wiadomości wysłanych do użytkowników.

Serwer:
- Nasłuchuje na przychodzące połączenia od klientów na wskazanym porcie.
- Obsługuje komunikację w formacie:
  ```
  1:UserNameSource:UserNameDestination:Message
  ```
  - `1` – identyfikator formatu wiadomości,
  - `UserNameSource` – nazwa nadawcy,
  - `UserNameDestination` – nazwa odbiorcy,
  - `Message` – treść wiadomości.
- **Przykład** wiadomości: `1:Klient1:Klient2:Hej`.
- Jeśli odbiorca jest **online**, wiadomości są wysyłane natychmiast.
- Jeśli odbiorca jest **offline**, wiadomości są przechowywane w bazie danych.
- Serwer jest **współbieżny** – każdy klient jest obsługiwany w osobnym wątku.
- Każda wiadomość jest zapisywana do bazy danych w celu archiwizacji konwersacji.

### Implementacja klienta

Klient został zaimplementowany w języku **C#** w popularnym i często używanym frameworku **WPF** do tworzenia dynamicznych aplikacji desktopowych.
Dla przejrzystości kodu skorzystaliśmy ze wzorca MVVM czyli **model-view**, **view**, **model** rozdzielając tym samym logię aplikacji od jej części graficznej.

---

#### Okna aplikacji

1. `Login view` - okno logowania pozwalające połączyć użytkownika z serwerem aplikacji oraz pobranie wiadomości zarchiwizowanych 
2. `Home view` - główne okno aplikacji po zalogowaniu przedstawiające wszystkie kontrolki, inicjujące zdarzania takie jak ciągłe pobieranie nowych wiadomości 
3. `UserSideBar view` - okno po lewej stronie przedstawiwające wszystkich użytkowników z którymi rozmawialiśmy do tej pory, obsługuje ona zdarzenia takie jak selekcja konkretnego użytkownika
4. `Messages view` - okno tylko dla wiadomości wyświetlające przeprowadzone konwersacje, pokazuje po prawej wysłane wiadomości z naszego konta, a po lewej wiadomości użytkownika do którego pisaliśmy
5. `Useradd view` - okno ,,Użytkownicy" które pozwala dodać nowego użytkownika do konwersacji

Przełaczanie między konkretnymi oknami odbywa się przez naciskanie odpowiednich przycisków na interfejsie.
Staraliśmy się aby interfejs był czytelny, lekki, minimalistczny i prosty w tym celu zastosowaliśmy paletę 5 kolorów:

- ![#c5ebff](https://placehold.co/15x15/#c5ebff/#c5ebff.png) `#c5ebff`
- ![#8cd6ff](https://placehold.co/15x15/#8cd6ff/#8cd6ff.png) `#8cd6ff`
- ![#052232](https://placehold.co/15x15/#052232/#052232.png) `#052232`
- ![#073148](https://placehold.co/15x15/#073148/#073148.png) `#073148`
- ![#EBF1F4](https://placehold.co/15x15/#EBF1F4/#EBF1F4.png) `#EBF1F4`

oraz czcionkę Rubik medium oraz regular.

### Wizualizacja klienta

![konwersacja klientów](https://git.cs.put.poznan.pl/projekt-sk2/gg/-/raw/development/Klienci.jpg)

## Kompilacja, uruchomienie i obsługa
### Kompilacja serwera na Linuxie
Aby skompilować serwer, użyj polecenia:
```bash
g++ -Wall ServerLinux.cpp -lsqlite3 -o [nazwa_wyjściowa]
```
> **Wymagania**:
> - Biblioteka `sqlite3` (instalacja: `sudo apt install sqlite3` na Ubuntu).
> - Flaga `-lsqlite3` w poleceniu kompilującym.

### Uruchomienie serwera
Aby uruchomić serwer wpisz w konsoli ścieżkę do pliku wykonywalnego.

### Uruchomienie klienta
Aby uruchomić klienta wyszukaj plik ClientWPF.exe, możesz również wprowadzić parametry: pierwszy z nich to:
1. ip serwera
2. port serwera

**Domyślna opcja** to ip adres 192.168.0.102 oraz port 8080

### Obsługa klienta
1. Po uruchomieniu aplikacji klienta użytkownik podaje **nazwę użytkownika** (nick) oraz klika **zaloguj**.
2. Po zalogowaniu użytkownik widzi ekran z wiadomościami.
3. Z lewej strony znajduje się **lista użytkowników**, z którymi prowadził rozmowy.
4. Po prawej na górze, obok przycisków funkcyjnych znajduję się nazwa obecnie zalogowanego użytkownika.
5. Aby rozpocząć rozmowę z nowym użytkownikiem:
   - Kliknij **„Użytkownicy”**.
   - Wpisz **nick odbiorcy**.
   - Kliknij przycisk **dodawania użytkownika**.
6. Powrót do ekranu wiadomości – kliknij **„Home”**.
7. Aby zmienić konto, kliknij **„Wyloguj”** a następnie zaloguj się na nowe konto.
8. Wysyłanie wiadomości:
   - Kliknięcie **Enter**.
   - Kliknięcie **ikonki dymku**.

---
