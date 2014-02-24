set BASEDIR=E:\Windows DDK\3790.1830\

del "%BASEDIR%\src\test\*.*" /Q
copy /Y *.* "%BASEDIR%\src\test\"
