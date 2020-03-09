Do wykonania klon Space Invaders (Unity 2019.3.0)

state machine (minimum 4 stany - intro, przygotowanie inwazji, gra, [pausa], end)
object pooling
event driven UI
external input files (statystyki dla przeciwników wczytywane z zewnętrznych plików, opcjonalnie uwzględniające wybrany poziom trudności)

Uproszczone zachowanie przeciwników - ruch lewo-prawo, ostrzał.
Trzy rodzaje przeciwników różniących się wyglądem i statystykami (częstotliwość strzału, dmg, reload time)
Poziomy mogą być losowane, wygląd planszy, statku, przeciwników nie ma znaczenia, bez save/load.
Czas na wykonanie testu to tydzień (będziemy liczyć od jutra :)). Wyślij tyle ile uda Ci się w tym czasie wykonać.

statystyki dla przeciwników mogą być np: HP, częstość strzelania, szybkość strzału, szansa na spawn - wczytane z pliku, tak aby po zbudowaniu builda można było łatwo tweekować statystyki, bez ECS.