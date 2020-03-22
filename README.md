# MyTest
**Simple test shell for small computer classes**

This mess of files were created in order to make a custom testing platform for the small computer classes.
So it's not very sophisticated. The idea was to make a test shell as simple and fast as possible and it should work under different OSes.
This system works more or less as intended.

**Main features are:**
1. Simple test file structure in a Key = Value manner
2. Tests can be encoded and decoded so you'll never face a situation when nobody knows what original test looks like
3. Each run test shell shuffles questions and answers
4. Students can traverse questions forwards and backwards
5. Tests can be limited in time
6. Tests can be performed in Loyal and Punish mode. Punish mode gives penalty for giving the wrong answers
7. Each question is meant to be a multichoice question
8. It was battle tested under Windows and Linux. In theory it should work under MacOS
9. Half-arsed english and russian UI language support

**Main disadvantages are:**
1. No image embeddings
2. No audio and video embeddings
3. Not all of the characters are supported
4. Needs more intelligent language support

## Test file structure
Every meaningful line has syntax: __Key = Value__

Spaces between the fields can be unlimited.

Commentary lines start from __#__ symbol.

**Rules to follow:**
1. Keywords __Test__, __Author__, __Date__, __Time__, __Mode__, __Question__ can be used in any order and in any place of the text
2. Keywords __Value__, __Answer__, __Answer+__ are ignored if placed before keyword __Question__
3. Keywords __Value__, __Answer__, __Answer+__ are considered to be related to the latest occurence of the __Question__ keyword

**Supported keywords:**
1. __Test__ - test name
2. __Author__ - Test author
3. __Date__ - Creation/Modification date. Must be in format *DD.MM.YYYY*
4. __Time__ - Test time in seconds. Note that this is a time for the whole test.
5. __Mode__ - Test mode. Can be either *Loyal* or *Punish*.
*Punish* mode means wrong answers give negative points worth question value divided on all wrong answers.
*Loyal* is the default mode and doesn't account negative points.
6. __Question__ - Each question begins with keyword **Question**
7. __Value__ - how much points is given question worth
8. __Answer__ - Every keyword **Answer** is counted towards the latest encountered **Question** keyword
9. __Answer+__ - Correct answers are marked by *+* suffix


## Example test file
Example test file can be found here -> https://github.com/s4rduk4r/MyTest/blob/master/MyTest/Tests/TestExample.mytest
