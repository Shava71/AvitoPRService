DROP TRIGGER IF EXISTS trg_reviewer_limit ON reviewer;
DROP FUNCTION IF EXISTS check_reviewer_limit();

CREATE FUNCTION check_reviewer_limit()
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

CREATE TRIGGER trg_reviewer_limit
BEFORE INSERT ON reviewer
FOR EACH ROW
EXECUTE FUNCTION check_reviewer_limit();