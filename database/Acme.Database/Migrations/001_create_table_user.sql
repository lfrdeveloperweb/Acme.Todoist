CREATE TABLE "user"
(
	"user_id"				varchar(32)		CONSTRAINT user_pk PRIMARY KEY,
	document_number			varchar(11)		NOT NULL CONSTRAINT user_document_number_un UNIQUE,
	name					varchar(128)	NOT NULL,
	birth_date				date			NULL,
	email					varchar(128)	NOT NULL,
	email_confirmed			BOOLEAN         NOT NULL DEFAULT FALSE,	
	phone_number			varchar(20)		NULL,
	phone_number_confirmed  BOOLEAN         NOT NULL DEFAULT FALSE,
	role_id					SMALLINT        NOT NULL,
	"user_name"				varchar(32)		NOT NULL,
	password_hash			varchar(128)	NOT NULL,
	last_login_at			timestamptz    	NULL,
    login_count				SMALLINT		NOT NULL	DEFAULT 0,
	access_failed_count		int				NOT NULL	DEFAULT 0,
    is_locked				BOOLEAN			NOT NULL	CONSTRAINT user_is_locked_df DEFAULT false,
	created_by_id			varchar(32)		NULL		CONSTRAINT user_user_created_by_fk REFERENCES "user",
	created_by_name			varchar(128)	NULL,
	created_at				timestamptz		NOT NULL,
	updated_by_id			varchar(32)		NULL		CONSTRAINT user_user_updated_by_fk REFERENCES "user",
	updated_by_name			varchar(128)	NULL,
	updated_at				timestamptz		NULL
);


/*

	INSERT INTO "user" (user_id, "name", email, role, "password", created_at)
		VALUES('anonymous', 'Anonymous', 'anonymous@todoist.com', 2, 'P@ssw0rd', CURRENT_TIMESTAMP);

*/