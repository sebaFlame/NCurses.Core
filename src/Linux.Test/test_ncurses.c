/* Utility to test type sizes on linux
 * build using
 * gcc -o test_ncurses test_ncurses.c -lncursesw
*/

#define NCURSES_WIDECHAR 1

#include <stddef.h>
#include <locale.h>
#include <string.h>
#include <wchar.h>
#include <stdlib.h>
#include <ncursesw/curses.h>

int main(){
	//setlocale(LC_ALL, "");

	size_t chTypeSize = sizeof(chtype);
	size_t attr_tSize = sizeof(attr_t);
	size_t cchar_tSize = sizeof(cchar_t);
	size_t wchar_tSize = sizeof(wchar_t);
	size_t wint_tSize = sizeof(wint_t);
	size_t mmask_tSize = sizeof(mmask_t);

	printf("wide char enabled: %d\n", NCURSES_WIDECHAR);
	printf("chtype size: %zu\n", chTypeSize);
	printf("attr_t size: %zu\n", attr_tSize);
	printf("mmask_t size: %zu\n", mmask_tSize);
	printf("wchar_t size: %zu\n", wchar_tSize);
	printf("wint_t size: %zu\n", wint_tSize);
	printf("cchar_t size: %zu\n", cchar_tSize);
	return 1;
}
