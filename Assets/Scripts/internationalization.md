# Internationalization
Internationalization is a way of designing software in a way allows it to be easily translated to many languages without changing any code. In Sokoban, language files are JSONs made up of many pairs of two elements. Here is an example of a pair:<br/>
`"mainscene.mainscreen.title.text": "Sokoban",`<br/>
The first part of the pair is called a "translation key". The translation key is a string that identifies a specific piece of text in the game. The second part is just called the translation. These two elements are associated to each other in the game. Thanks to this system text in the game can be referenced as a translation key and only later translated.

# Adding languages
Adding new languages to Sokoban is very simple:
1. [Build](building.md) the project. 
2. Open the "lang" folder in the build and create a new file called "yourlanguage.json" (replace "yourlanguage" with the language name)
3. Paste the contents of "English.json" into your newly created file.
4. Replace the translation for the key "language.name" with the name of the language you are creating the translation for.
5. Now simply change the translation for each key from English into your language of choice.
6. Save the file and the new language will appear in game after a restart.