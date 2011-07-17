;; Copyright (c) 2011 ChunkFS Contributors
;;
;; The contents of this file are subject to the terms of the
;; Common Development and Distribution License, Version 1.0 only
;; (the "License").  You may not use this file except in compliance
;; with the License.
;;
;; A copy of the License is included in the file license.txt 
;; A copy of the License can also be obtained from the Open Source 
;; Initiative at http:;;www.opensource.org/licenses/cddl1.txt
;; 
;; See the License for the specific language governing permissions
;; and limitations under the License.
;;
;; When distributing Covered Code, include this notice in each
;; source code file and include the referenced License file, and  
;; if applicable, add a description of any modifications to the 
;; Summary of Contributions.
;;
;;
;; Summary of Contributions
;;
;; Date		 	Author			Organization			Description of contribution
;; ========== 	===============	=======================	=====================================================================
;; 07/15/2011 	T Shanley		West Leitrim Software	Original contribution - entire file; with derivation described below.
;;

; installer for ChunkFS
; compile with MakeNSISW (nullsoft installer make tool for windows)

; this code is derived in part from example code provided on the NSIS website at http://nsis.sourceforge.net/Examples/Modern%20UI/StartMenu.nsi
; this code is substantially altered from the original example 
; the original example contains the folowing byline:
;;;Written by Joost Verburg
; the original example is subject to the following license:
;;;This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
;;;
;;;Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
;;;
;;;   1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
;;;   2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
;;;   3. This notice may not be removed or altered from any source distribution. 



;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  !include "FileFunc.nsh"

;--------------------------------
;General

  ;Name and file
  !define APPNAME "ChunkFS"
  !define BUILD "bin\Debug"  
  ;!define BUILD "obj\Debug"  
  ;!define BUILD "obj\Release"  
  Name "${APPNAME}"
  ;OutFile "setup${APPNAME}.exe"
  OutFile "setup${APPNAME}.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES32\${APPNAME}"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\${APPNAME}" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Variables

  Var StartMenuFolder
  Var Dialog
  Var Label

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages
  !insertmacro MUI_PAGE_WELCOME 
  Page custom ListPreReq 
  !insertmacro MUI_PAGE_LICENSE "license_u.txt"
  !insertmacro MUI_PAGE_LICENSE "dokan.net-license.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\${APPNAME}" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
  
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
  
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Functions
Function ListPreReq
  !insertmacro MUI_HEADER_TEXT "Pre-Requisites" "Please review the pre-requisites before installing ChunkFS"

    nsDialogs::Create 1018
	Pop $Dialog

	${If} $Dialog == error
		Abort
	${EndIf}

	${NSD_CreateLabel} 0 0 100% 14u "Before installing ChunkFS, be sure you have installed:"
	Pop $Label

	${NSD_CreateLabel} 20 16 100% 14u "Microsoft .NET Framework version 2.0"
	Pop $Label

	${NSD_CreateLabel} 20 32 100% 14u "Dokan version 0.6."
	Pop $Label

	nsDialogs::Show
FunctionEnd

Function .onInit
  IntOp $0 ${SF_SELECTED} | ${SF_RO}
  SectionSetFlags SecCore $0
FunctionEnd

;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "ChunkFS core (Required)" SecCore

  SetOutPath "$INSTDIR"
  
  File readme.txt
  File license.txt
  File dokan.net-license.txt
  File ..\ChunkFSgui\${BUILD}\ChunkFSgui.exe
  File ..\ChunkFSgui\${BUILD}\ChunkFSgui.pdb
  File ..\ChunkFSLib\${BUILD}\ChunkFSLib.dll
  File ..\ChunkFSLib\${BUILD}\ChunkFSLib.pdb
  File ..\DokanNet\${BUILD}\DokanNet.dll
  File ..\DokanNet\${BUILD}\DokanNet.pdb
  
  ;Store installation folder
  WriteRegStr HKCU "Software\${APPNAME}" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  SetShellVarContext all
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
	
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\ChunkFS.lnk" "$INSTDIR\ChunkFSgui.exe"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\License.lnk" "$INSTDIR\license.txt"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\License - Dokan.NET.lnk" "$INSTDIR\dokan.net-license.txt"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Readme.lnk" "$INSTDIR\readme.txt"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END
  ${RefreshShellIcons}

SectionEnd

Section "ChunkFS tools" SecTools

  SetOutPath "$INSTDIR"
  
  File ..\CFSRestore\${BUILD}\CFSRestore.exe
  File ..\CFSRestore\${BUILD}\CFSRestore.pdb
  File ..\ChunkFS\${BUILD}\ChunkFS.exe
  File ..\ChunkFS\${BUILD}\ChunkFS.pdb
   
  SetShellVarContext all
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\ChunkFS Command Line.lnk" "cmd.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END
  ${RefreshShellIcons}

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecCore ${LANG_ENGLISH} "Includes ChunkFS library and GUI, and the Dokan.NET library."
  LangString DESC_SecTools ${LANG_ENGLISH} "Includes ChunkFS command line utilities."

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecCore} $(DESC_SecCore)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecTools} $(DESC_SecTools)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END
 
;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\CFSRestore.exe"
  Delete "$INSTDIR\CFSRestore.pdb"
  Delete "$INSTDIR\ChunkFS.exe"
  Delete "$INSTDIR\ChunkFS.pdb"
  Delete "$INSTDIR\ChunkFSgui.exe"
  Delete "$INSTDIR\ChunkFSgui.pdb"
  Delete "$INSTDIR\ChunkFSLib.dll"
  Delete "$INSTDIR\ChunkFSLib.pdb"
  Delete "$INSTDIR\DokanNet.dll"
  Delete "$INSTDIR\DokanNet.pdb"
  Delete "$INSTDIR\license.txt"
  Delete "$INSTDIR\dokan.net-license.txt"
  Delete "$INSTDIR\readme.txt"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
    
  Delete "$SMPROGRAMS\$StartMenuFolder\ChunkFS.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\ChunkFS Command Line.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\License.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\License - Dokan.NET.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Readme.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  ${RefreshShellIcons}
  
  DeleteRegKey /ifempty HKCU "Software\${APPNAME}"
  
  ; not working on win7, entries still showing; try again
  ${RefreshShellIcons}

SectionEnd