CREATE TABLE membership
(
	membership_id			varchar(32)		CONSTRAINT membership_pk PRIMARY KEY,
	name					varchar(128)	NOT NULL,
	role_id					SMALLINT        NOT NULL,
	access_failed_count		INT				NOT NULL	DEFAULT 0,
    locked_at				timestamptz		NULL,
	created_by				varchar(32)		NULL		CONSTRAINT membership_membership_created_by_fk REFERENCES membership,
	created_at				timestamptz		NOT NULL,
	updated_by				varchar(32)		NULL		CONSTRAINT membership_membership_updated_by_fk REFERENCES membership,
	updated_at				timestamptz		NULL
);


/*

	INSERT INTO membership (user_id, "name", email, role, "password", created_at)
		VALUES('anonymous', 'Anonymous', 'anonymous@todoist.com', 2, 'P@ssw0rd', CURRENT_TIMESTAMP);

*/