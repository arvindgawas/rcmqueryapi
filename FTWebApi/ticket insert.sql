


[Ticket](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BatchID] [varchar](50) NOT NULL,
	[TicketID] [varchar](50) NOT NULL,
	[ticketdate] [date] NOT NULL,

	insert into Ticket (batchid,ticketid,ticketdate,tikcettime)
	values ('111','1','2015-05-21','11:20')


	select top 1000 * from Ticket

	delete from Ticket



exec sp_executesql N'INSERT [dbo].[Ticket]([BatchID], [TicketID], [ticketdate], [tikcettime], [tikcettype], [assignedto], [acceptstatus], [resolveddate], 
[customer], [pickupcode], [clientcode], [crnno], [client], [area], [cdpncm], [region], [location], [hublocation], [problem], [mistakedoneby], [errortype], [querystatus], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
VALUES (@0, @1, @2, @3, NULL, @4, @5, @6, NULL, @7, @8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @9, @10, @11, NULL, NULL, NULL, NULL)
SELECT [ID]
FROM [dbo].[Ticket]
WHERE @@ROWCOUNT > 0 AND [TicketID] = @1',N'@0 varchar(50),@1 varchar(50),@2 datetime2(7),@3 time(7),@4 varchar(50),
@5 varchar(50),@6 datetime2(7),@7 varchar(50),@8 varchar(50),@9 varchar(50),@10 varchar(50),@11 varchar(50)'
,@0='112',@1='1112',@2='2021-05-28 00:00',@3='12:15:00',@4='111',@5='Accept',@6='2021-05-28',@7='421306',@8='222',@9='Location',@10='1111',@11='Close'
	