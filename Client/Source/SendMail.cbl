      *{Bench}prg-comment
      * SendMail.cbl
      * SendMail.cbl is generated from C:\GitRepo\communications\Client\SendMail.Psf
      *{Bench}end
       IDENTIFICATION              DIVISION.
      *{Bench}prgid
       PROGRAM-ID. SendMail.
       AUTHOR. Edgar.Irle.
       DATE-WRITTEN. Thursday, 22 August 2019 09:01:12.
       REMARKS. 
      *{Bench}end
       ENVIRONMENT                 DIVISION.
       CONFIGURATION               SECTION.
       SPECIAL-NAMES.
      *{Bench}activex-def
      *{Bench}end
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
       COPY "showmsg.def".
      *{Bench}end

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
       
       A000-MAIN.

           PERFORM A100-INITIAL.

           PERFORM Acu-Main-Scrn.

       A000-ACCEPT.
           ACCEPT Main

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
           STOP RUN.

       A100-INITIAL.

       A100-EXIT.
           EXIT.

       B100-SEND-MAIL SECTION.

       B100-EXIT.
           EXIT.

       B200-QUERY-REPORT SECTION.

       B200-EXIT.
           EXIT.

       TERMINATION SECTION.
      *{Bench}copy-procedure
       COPY "showmsg.cpy".
       COPY "SendMail.prd".
       COPY "SendMail.evt".
      *{Bench}end
       REPORT-COMPOSER SECTION.
