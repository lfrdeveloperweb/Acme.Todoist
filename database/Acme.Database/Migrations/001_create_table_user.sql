CREATE TABLE "user"
(
	"user_id"		varchar(32)		CONSTRAINT user_pk PRIMARY KEY,
	name			varchar(128)	NOT NULL,
	email			varchar(128)	NOT NULL,
	birthday		date			NULL,
	phone_number	varchar(20)		NULL,
	password		varchar(128)	NOT NULL,
	created_by		varchar(32)		NULL	CONSTRAINT user_user_created_by_fk REFERENCES "user",
	created_at		timestamptz		NOT NULL,
	updated_by		varchar(32)		NULL	CONSTRAINT user_user_updated_by_fk REFERENCES "user",
	updated_at		timestamptz		NULL,
);


/*

	INSERT INTO "user" (user_id, "name", email, "password", created_at)
		VALUES('1234', 'Valerie Welch', 'valerie.welch@acme.com', 'P@ssw0rd', CURRENT_TIMESTAMP);

*/