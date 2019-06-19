FOR /F "tokens=1-3 delims=:." %%a IN ("%time%") DO (SET _MyTime=%%a%%b%%c)
SET _MyTime=%_MyTime: =0%
set F_YMD=%1
set SYS_YMDHMS=%DATE:~0,4%%DATE:~5,2%%DATE:~8,2%_%_MyTime%
set SYS_YMD=%DATE:~0,4%%DATE:~5,2%%DATE:~8,2%
set Src_PATH=http://www.jpx.co.jp/english/markets/derivatives/trading-volume/tvdivq00000014nn-att/
set Src_FILE1=%F_YMD%open_interest_e.xls
set Src_FILE2=%F_YMD%_market_data_whole_day_e.xls
::Path
::傳入gs_ap_path像是C:\CIN這樣
set SYS_PATH=%2
set Flag_PATH=%SYS_PATH%ErrSP\20110_JTX_flag.txt
set Trg_PATH=%SYS_PATH%Report\%SYS_YMD%\

::判斷目錄存在
if exist %Flag_PATH% (del %Flag_PATH%) 
if not exist %Trg_PATH% (md %Trg_PATH%) 

::判斷檔案存在
if exist %Trg_PATH%%Src_FILE1% (move %Trg_PATH%%Src_FILE1% %Trg_PATH%%Src_FILE1%_bak%SYS_YMDHMS%) 
if exist %Trg_PATH%%Src_FILE2% (move %Trg_PATH%%Src_FILE2% %Trg_PATH%%Src_FILE2%_bak%SYS_YMDHMS%) 

::Copy File
bitsadmin.exe  /transfer mydownloadjob1  /download  /priority normal %Src_PATH%%Src_FILE1%  %Trg_PATH%%Src_FILE1% > %Trg_PATH%20110_TJX_%SYS_YMDHMS%.log
bitsadmin.exe  /transfer mydownloadjob2  /download  /priority normal %Src_PATH%%Src_FILE2%  %Trg_PATH%%Src_FILE2% >> %Trg_PATH%20110_TJX_%SYS_YMDHMS%.log

::End
echo XXX > %Flag_PATH%

