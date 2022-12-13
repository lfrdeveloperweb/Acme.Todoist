CREATE TABLE todo_comment 
(
	todo_comment_id		int				constraint todo_comment_pk primary key generated always as identity,
	todo_id				varchar(32)		NOT NULL CONSTRAINT todo_comment_todo_fk REFERENCES todo,
	description			varchar(128)	NOT NULL,
	created_by			varchar(32)		NOT NULL CONSTRAINT todo_comment_user_updated_by_fk REFERENCES "user",
	created_at			timestamptz		NOT NULL	
);