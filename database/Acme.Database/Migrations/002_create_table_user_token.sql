CREATE TABLE "user_token"
(
	"user_id"		varchar(32)		NOT NULL,
	"type"			SMALLINT		NOT NULL,
	"value"			varchar(32)		NOT NULL,
	"data"			json			NULL,
	expires_at		timestamptz    	NOT NULL,	

	CONSTRAINT user_token_pk PRIMARY KEY("user_id", "type")
);