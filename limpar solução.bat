@echo off

RMDIR /Q /S > NUL "Source/ControleAcesso/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso/obj"
RMDIR /Q /S > NUL "Source/ControleAcesso.Android/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso.Android/obj"
RMDIR /Q /S > NUL "Source/ControleAcesso.iOS/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso.iOS/obj"

echo "RODANDO A SEGUNDA VEZ"

RMDIR /Q /S > NUL "Source/ControleAcesso/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso/obj"
RMDIR /Q /S > NUL "Source/ControleAcesso.Android/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso.Android/obj"
RMDIR /Q /S > NUL "Source/ControleAcesso.iOS/bin"
RMDIR /Q /S > NUL "Source/ControleAcesso.iOS/obj"

exit