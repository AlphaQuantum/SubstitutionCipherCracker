# Substitution Cipher Cracker
This is a simple winforms application to help you crack a message cracked by the substitution cipher without knowing the key
It is by no means perfect and completely autonomus, but it speeds up the time to crack the message. 
## **How it works**
The program takes the longest word and swaps the letters of that word with another word of the same lenght in the text file(Why the longest word? because longer words are less common, and have more characters to swap the ciphred text with). After that if there are weird 2 letter words it filters them (in the italian language words most 2 character words are fixed and few, for example there isn't RC, so the program will remove that).
After the program is done with the first word, it will do the same with all other words, but as you might guess the shorter the word is, the less information will be given, but nonetheless there's still some value to be extracted ;)
