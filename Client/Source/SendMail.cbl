      *{Bench}prg-comment
      * SendMail.cbl
      * SendMail.cbl is generated from C:\GitRepo\communications\Client\SendMail.Psf
      *{Bench}end
       IDENTIFICATION              DIVISION.
      *{Bench}prgid
       PROGRAM-ID. SendMail.
       AUTHOR. Edgar.Irle.
       DATE-WRITTEN. Thursday, 22 August 2019 11:39:42.
       REMARKS. 
      *{Bench}end
       ENVIRONMENT                 DIVISION.
       CONFIGURATION               SECTION.
       SPECIAL-NAMES.
      *{Bench}activex-def
      *{Bench}end
      ***** COPY "BackupLib.def"..
      *{Bench}decimal-point
      *{Bench}end
       INPUT-OUTPUT                SECTION.
       FILE-CONTROL.
      *{Bench}file-control
      *{Bench}end
       DATA                        DIVISION.
       FILE                        SECTION.
      *{Bench}file
      *{Bench}end
       WORKING-STORAGE             SECTION.
      *{Bench}acu-def
       COPY "acugui.def".
       COPY "acucobol.def".
       COPY "crtvars.def".
       COPY "fonts.def".
       COPY "showmsg.def".
      *{Bench}end

      ***** 77  BACKUP-HANDLE usage is handle of   
      *****      "@BackupLib.BackupLib.BackupClass".

       01  WS-VENDOR-KEY               PIC X(40).
       01  SUB                         PIC 999.
       01  WS-POINTER                  PIC 9(5).
       01  WS-LINE-VALUE               PIC X(100).

       01  WS-MAIL-REC.
           05  WS-MAIL-FROM            PIC X(100).
           05  WS-MAIL-TO              PIC X(500).
           05  WS-MAIL-SUBJECT         PIC X(500).
           05  WS-MAIL-BODY            PIC X(1000).

       01  WS-REPORT-REC.
           05  WS-REPORT-STRING        PIC X(10000).
           05  WS-REPORT-LINES-TABLE.
               10  WS-REPORT-LINES OCCURS 100.
                   15 WS-REPORT-DATE1  PIC X(20).
                   15 WS-REPORT-DATE2  PIC X(20).
                   15 WS-REPORT-COND   PIC X(20).
                   15 FILLER           PIC X(40).

      *{Bench}copy-working
       COPY "SendMail.wrk".
      *{Bench}end
       LINKAGE                     SECTION.
      *{Bench}linkage
      *{Bench}end
       SCREEN                      SECTION.
      *{Bench}copy-screen
       COPY "SendMail.scr".
      *{Bench}end

      *{Bench}linkpara
       PROCEDURE DIVISION.
      *{Bench}end
      *{Bench}declarative
      *{Bench}end
      ****************************************************************** 
       A000-MAIN SECTION.
      ********************************

           PERFORM ACU-INIT-FONT.

           PERFORM A100-INITIAL.

           PERFORM Acu-Main-Scrn.

       A000-ACCEPT.
           ACCEPT Main.

           EVALUATE TRUE
               WHEN Exit-Pushed
                   GO TO A000-EXIT
               WHEN sendmail-pushed
                   PERFORM B100-SEND-MAIL
               WHEN queryreport-pushed
                   PERFORM B200-QUERY-REPORT
           END-EVALUATE.

           GO TO A000-ACCEPT.

       A000-EXIT.
      ********************************
           STOP RUN.

      ****************************************************************** 
       A100-INITIAL SECTION.
      ********************************

           ACCEPT WS-VENDOR-KEY FROM ENVIRONMENT "X_VENDOR_KEY".

       A100-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B100-SEND-MAIL SECTION.
      ********************************

           PERFORM B101-INITIAL.

           PERFORM Acu-SendMail-Scrn.

           DISPLAY MESSAGE WS-MAIL-FROM.

       B100-ACCEPT.
           
           ACCEPT SendMail.

           EVALUATE TRUE
               WHEN Exit-Pushed
                    GO TO B100-LAST
               WHEN submit-pushed
                    PERFORM B110-SEND-MAIL
               WHEN response-pushed
                    PERFORM B120-QUERY-RESPONSE
           END-EVALUATE.

           GO TO B100-ACCEPT.

       B100-LAST.
           DESTROY SendMail SM-Handle.

       B100-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B101-INITIAL SECTION.
      ********************************

           INITIALIZE WS-MAIL-REC.

           ACCEPT WS-MAIL-FROM FROM ENVIRONMENT "X_MAIL_FROM".

       B101-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B110-SEND-MAIL SECTION.
      ********************************
           
           PERFORM C100-CREATE-MAIL-HANDLE.

           PERFORM C200-MODIFY-MAIL-HANDLE-SEND.

           PERFORM C201-INQUIRE-MAIL-SEND.

       B110-FINAL.

           PERFORM C101-DESTROY-MAIL-HANDLE.

       B110-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B111-MODIFY-MAIL-HANDLE SECTION.
      ********************************

      *****       inquire BACKUP-HANDLE EventStatus 
      *****            In WS-PROG
      *****       MOVE SPACES TO backup-per-lbl-val
      *****       inquire BACKUP-HANDLE CurrentFile 
      *****            In WS-CUR-FILE
      *****       STRING "Busy with Company " DELIMITED BY SIZE
      *****              LS-CO(SUB1)          DELIMITED BY SIZE
      *****              " - File: "          DELIMITED BY SIZE
      *****              WS-CUR-FILE          DELIMITED BY "  "
      *****              INTO backup-per-lbl-val 
      *****       DISPLAY backup-per-lbl
      *****       MODIFY Progfr, FILL-PERCENT WS-PROG
      *****       inquire BACKUP-HANDLE ErrorStatus 
      *****            In con-ErrorStatus


       B111-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B120-QUERY-RESPONSE SECTION.
      ********************************

           PERFORM C100-CREATE-MAIL-HANDLE.

           PERFORM C203-INQUIRE-MAIL-RESPONSE.

       B120-FINAL.

           PERFORM C101-DESTROY-MAIL-HANDLE.

       B120-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B200-QUERY-REPORT SECTION.
      ********************************

           PERFORM B201-GET-VALUES.
           PERFORM Acu-Report-Scrn.

       B200-ACCEPT.
           
           ACCEPT Report.

           EVALUATE TRUE
               WHEN Exit-Pushed
                    GO TO B200-LAST
           END-EVALUATE.

           GO TO B200-ACCEPT.

       B200-LAST.
           DESTROY Report Report-Handle.

       B200-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       B201-GET-VALUES SECTION.
      ********************************

           PERFORM C100-CREATE-MAIL-HANDLE.

           PERFORM C202-INQUIRE-MAIL-REPORT.

       B201-FINAL.

           PERFORM C101-DESTROY-MAIL-HANDLE.


       B201-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C100-CREATE-MAIL-HANDLE SECTION.
      ********************************

      *     ACCEPT TERMINAL-ABILITIES FROM TERMINAL-INFO.
      *     ACCEPT WS-SERVER-NAME FROM ENVIRONMENT "WS-SERVER-NAME".
      *     create "@BackupLib",
      *     NAMESPACE IS "BackupLib",
      *     CLASS-NAME IS "BackupClass"              
      *     handle is BACKUP-HANDLE
      *     SERVER-NAME IS ws-server-name.

       C100-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C101-DESTROY-MAIL-HANDLE SECTION.
      ********************************
           
      *****     DESTROY S3BACKUP-HANDLE.

       C101-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C200-MODIFY-MAIL-HANDLE-SEND SECTION.
      ********************************

      *****     MODIFY S3BACKUP-HANDLE "@SendMail" 
      *****           (WS-MAIL-KEY,
      *****            WS-MAIL-FROM,
      *****            WS-MAIL-TO,
      *****            WS-MAIL-BODY) GIVING WS-DELL-STATUS.
      *****
      *****     INQUIRE S3BACKUP-HANDLE ErrorMessage in WS-DELL-STATUS.

       C200-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C201-INQUIRE-MAIL-SEND SECTION.
      ********************************

      *****       inquire BACKUP-HANDLE EventStatus 
      *****            In WS-PROG
      *****       MOVE SPACES TO backup-per-lbl-val
      *****       inquire BACKUP-HANDLE CurrentFile 
      *****            In WS-CUR-FILE
      *****       STRING "Busy with Company " DELIMITED BY SIZE
      *****              LS-CO(SUB1)          DELIMITED BY SIZE
      *****              " - File: "          DELIMITED BY SIZE
      *****              WS-CUR-FILE          DELIMITED BY "  "
      *****              INTO backup-per-lbl-val 
      *****       DISPLAY backup-per-lbl
      *****       MODIFY Progfr, FILL-PERCENT WS-PROG
      *****       inquire BACKUP-HANDLE ErrorStatus 
      *****            In con-ErrorStatus

       C201-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C202-INQUIRE-MAIL-REPORT SECTION.
      ********************************

      *****       inquire BACKUP-HANDLE EventStatus 
      *****            In WS-PROG
      *****       MOVE SPACES TO backup-per-lbl-val
      *****       inquire BACKUP-HANDLE CurrentFile 
      *****            In WS-CUR-FILE
      *****       STRING "Busy with Company " DELIMITED BY SIZE
      *****              LS-CO(SUB1)          DELIMITED BY SIZE
      *****              " - File: "          DELIMITED BY SIZE
      *****              WS-CUR-FILE          DELIMITED BY "  "
      *****              INTO backup-per-lbl-val 
      *****       DISPLAY backup-per-lbl
      *****       MODIFY Progfr, FILL-PERCENT WS-PROG
      *****       inquire BACKUP-HANDLE ErrorStatus 
      *****            In con-ErrorStatus

           MOVE 1 TO WS-POINTER.
           ACCEPT WS-REPORT-STRING FROM ENVIRONMENT "REPORT_STRING".

           MOVE "Date Submitted" TO WS-REPORT-DATE1(1)
           MOVE "Date Processed" TO WS-REPORT-DATE2(1)
           MOVE "Processed"      TO WS-REPORT-COND(1).

           MOVE "--------------" TO WS-REPORT-DATE1(2)
           MOVE "--------------" TO WS-REPORT-DATE2(2)
           MOVE "---------"      TO WS-REPORT-COND(2).

           PERFORM VARYING SUB FROM 4 BY 1 UNTIL SUB > 100
                 MOVE SPACES TO WS-LINE-VALUE
                 UNSTRING WS-REPORT-STRING DELIMITED BY "|"
                          INTO WS-LINE-VALUE
                          WITH POINTER WS-POINTER
                 IF WS-LINE-VALUE NOT = SPACES
                     UNSTRING WS-LINE-VALUE DELIMITED BY ";"
                              INTO WS-REPORT-DATE1(SUB)
                                   WS-REPORT-DATE2(SUB)
                                   WS-REPORT-COND(SUB)
                 ELSE
                     EXIT PERFORM
                 END-IF
           END-PERFORM.

           MOVE WS-REPORT-LINES-TABLE TO Scr-Report-Item.

       C202-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       C203-INQUIRE-MAIL-RESPONSE SECTION.
      ********************************
       
      *****       inquire BACKUP-HANDLE EventStatus 
      *****            In WS-PROG
      *****       MOVE SPACES TO backup-per-lbl-val
      *****       inquire BACKUP-HANDLE CurrentFile 
      *****            In WS-CUR-FILE
      *****       STRING "Busy with Company " DELIMITED BY SIZE
      *****              LS-CO(SUB1)          DELIMITED BY SIZE
      *****              " - File: "          DELIMITED BY SIZE
      *****              WS-CUR-FILE          DELIMITED BY "  "
      *****              INTO backup-per-lbl-val 
      *****       DISPLAY backup-per-lbl
      *****       MODIFY Progfr, FILL-PERCENT WS-PROG
      *****       inquire BACKUP-HANDLE ErrorStatus 
      *****            In con-ErrorStatus

       C203-EXIT.
      ********************************
           EXIT.

      ****************************************************************** 
       TERMINATION SECTION.
      *{Bench}copy-procedure
       COPY "showmsg.cpy".
       COPY "SendMail.prd".
       COPY "SendMail.evt".
      *{Bench}end
       REPORT-COMPOSER SECTION.
