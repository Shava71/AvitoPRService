CREATE OR REPLACE FUNCTION check_reviewer_limit()
RETURNS trigger AS $$
BEGIN
    IF (
        SELECT COUNT(*) FROM reviewer
        WHERE pull_request_id = NEW.pull_request_id
    ) >= 2 THEN
        RAISE EXCEPTION 'Too many reviewers for PR';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_reviewer_limitне
BEFORE INSERT ON reviewer
FOR EACH ROW
EXECUTE FUNCTION check_reviewer_limit();