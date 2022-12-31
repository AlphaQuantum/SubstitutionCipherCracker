# Substitution Cipher Cracker

This is a simple Windows Forms application that helps you crack a message that has been encrypted using the substitution cipher, even if you do not know the key. It is not perfect and does not work completely autonomously, but it can speed up the time it takes to crack the message.
>Note: This software works with italian sentences

### Technologies Used:
C# is a popular programming language that is commonly used for building Windows applications. It is an object-oriented language that is easy to learn and has a large developer community.
Windows Forms (WinForms) is a user interface (UI) framework that is used to build graphical user interface (GUI) applications for the Windows operating system. It provides a set of classes and controls that allow developers to design and build interactive forms and dialog boxes. WinForms is a powerful and flexible way to create UIs, and it is widely used in the development of desktop applications.

### How it works
The program takes the longest word in the message and swaps the letters of that word with another word of the same length from a text file. The reason for using the longest word is that longer words are less common and have more characters that can be used to try and decrypt the message. After the program has done this with the longest word, it will do the same with all of the other words. However, the shorter the word is, the less information it will provide. Nonetheless, there is still some value to be extracted from these shorter words. Additionally, the program will filter out any strange two-letter words, as these are often not found in normal language and may not provide any useful information for decryption.
