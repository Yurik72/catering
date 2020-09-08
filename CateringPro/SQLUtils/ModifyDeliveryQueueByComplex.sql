ALTER TABLE dbo.DeliveryQueues ADD
	ComplexId int NOT NULL CONSTRAINT DF_DeliveryQueues_ComplexId DEFAULT 0
	go