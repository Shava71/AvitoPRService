CREATE OR REPLACE FUNCTION check_reviewer_limit()
RETURNS trigger AS $$
BEGIN
    IF (
        SELECT COUNT(*) FROM reviewers
        WHERE pull_request_id = NEW.pull_request_id
    ) >= 2 THEN
        RAISE EXCEPTION 'Too many reviewers for PR';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_reviewer_limit
BEFORE INSERT ON reviewers
FOR EACH ROW
EXECUTE FUNCTION check_reviewer_limit();