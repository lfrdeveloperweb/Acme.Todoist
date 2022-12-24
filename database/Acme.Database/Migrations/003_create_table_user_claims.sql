CREATE TABLE user_claim
(
    user_claim_id   int                  CONSTRAINT todo_comment_pk primary key generated always as identity,
    user_id         VARCHAR(32) NOT NULL CONSTRAINT user_claim_user_fk REFERENCES "user" (id) ON DELETE CASCADE,
    claim_type      text NOT NULL,
    claim_value     text NOT NULL    
);