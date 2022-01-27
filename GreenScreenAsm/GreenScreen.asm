
 ;  Temat projektu: GreenScreen - usuwanie wybranego koloru ze zdjecia
 ;  Autor:          Jakub Polczyk
 ;  Semestr:        5
 ;  Rok Akademicki: 2021/2022
 ;  Wersja:         Final Ultimate 1.0

.data
	dqwColorArray db 16 dup (?)									;Tablica, ktora uzupelni rejestr xmm bajtami koloru wybranego przez uzytkownika do usuniecia
	
.code
removeGreenScreenAsm PROC
;Przeniesienie danych wejsciowych z wywolania funkcji do rejestrow
		MOV r9, rcx												;ciag pikseli ARGB w bajtach
		MOV r10, rdx											;kolor do usuniecia
		MOV r11, r8												;liczba bajtow, rozmiar tablicy pikseli rejestru r9

		XOR rcx, rcx											;czyszczenie licznika
		MOV rax, OFFSET dqwColorArray							;przeniesienie adresu tablicy do rax

;wypelnienie dqwColorArray kolorem do usuniecia
;Kolejnosc: ARGB
xmmFilerLoop:
;Kanal alfa jest sztywno ustawiony na 255 i nie pobierany z GUI
		XOR r12,r12
		NOT r12
		MOV [rax], r12b
		
;wartosc R pobierana z GUI zapisywana do tablicy
		ADD rax,TYPE dqwColorArray 
		MOV r12b, [r10]
		MOV [rax], r12b
		
;wartosc G pobierana z GUI zapisywana do tablicy
		ADD rax,TYPE dqwColorArray 
		MOV r12b, [r10+1]
		MOV [rax], r12b

;wartosc B pobierana z GUI zapisywana do tablicy
		ADD rax,TYPE dqwColorArray 
		MOV r12b, [r10+2]
		MOV [rax], r12b

;przejscie dalej w dqwColorArray i inkrementacja licznika o 4
		ADD rax,TYPE dqwColorArray 
		ADD rcx, 4

;sprawdzenie czy cala tablica jest wypelniona
		CMP rcx, 16
		JNE xmmFilerLoop
		
;uzycie tablicy do wypelnienia rejestru xmm0
		MOV rax, OFFSET dqwColorArray
		MOVDQU xmm0, [rax]										;przeniesienie 4 pikseli (16 bajtow) z tablicy pikseli do xmm1

;przetwarzanie obrazu

;obraz mniejszy niz 4px (16 bajtow)
		CMP r11, 16
		JB lessThen16											;skok jezeli mniejszy
		
		MOV rax, r9												;przeniesienie tablicy pikseli do RAX

sseLoop:		
		MOVDQU xmm1, [rax]										;przniesienie 4 pikseli (16 bajtow) z tablicy pikseli do xmm
		PCMPEQD xmm0, xmm1										;sprawdzenie czy cztery piksele sa rowne, jezeli tak zamiana bitow pikseli na 1 w xmm0
		PCMPEQD xmm2, xmm2										;ustawienie wszystkich bitow xmm2 na 1
		PXOR xmm0,xmm2											;PXOR + poprzednia instrukcja, negacja bitowa bitow xmm0
		PAND xmm1, xmm0											;wyczyszczenie odpowiednich bitow w xmm1
		MOVDQU [rax], xmm1										;zapisanie zmienionych bitow do tablicy

		ADD rax, 16												;przejscie do kolejnych pikseli w tablicy
		
		SUB r11, 16												;zmniejszenie nieprzetworzonych bajtow o 16
		CMP r11, 0												;sprawdzenie czy ilosc bajtow jest wystarczajaca do wypelnienia rejestru xmm
		JLE restPixelLoop
		
		
;uzupelnienie xmm0
		MOV r13, OFFSET dqwColorArray 
		MOVDQU xmm0, [r13]
		JMP sseLoop

;odpowiedzialny za reszte pikseli (zostalo mniej niz 16)
restPixelLoop:
;przetworzenie pozostalych
		add r11, 16

	
lessThen16:
;wyjscie jezeli wszystkie zostaly przetworzone
		CMP r11, 0 
		JZ exit	

;sprawdzenie A
		mov r12b, 255
		mov r13b, [rax]
		cmp [rax],r12b
		jne bypassPixel

;sprawdzenie R
		mov r12b, [r10]
		mov r13b, [rax + 1]
		cmp [rax + 1], r12b
		jne bypassPixel

;sprawdzenie G
		mov r12b, [r10 + 1]
		mov r13b, [rax + 2]
		cmp [rax + 2], r12b
		jne bypassPixel

;sprawdzenie B
		mov r12b, [r10 + 2]
		mov r13b, [rax + 3]
		cmp [rax + 3], r12b
		jne bypassPixel
		
;wyczyszczenie piksela
		xor r14,r14
		mov [rax],r14b
		mov [rax+1],r14b
		mov [rax+2],r14b
		mov [rax+3],r14b
		mov r13b,[rax]
		mov r13b,[rax+1]
		mov r13b,[rax+2]
		mov r13b,[rax+3]

bypassPixel:
;jezeli jakis bajt piksela nie jest rowny kolorowi do usuniecia przejdz do nastepnego piksela
		sub r11, 4
		add rax, 4
		jmp lessThen16

exit:
mov rax,0
ret

removeGreenScreenAsm ENDP

end
