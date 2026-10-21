-- Community rankings: each app install sends its ranked list anonymously.
-- Run once on the production database BEFORE deploying the API that uses it.
-- DeviceId is a random id the app generates; it isn't tied to a person or account.
CREATE TABLE IF NOT EXISTS Rankings (
  RankingId  BIGINT AUTO_INCREMENT PRIMARY KEY,
  DeviceId   CHAR(36)     NOT NULL,
  PlaceId    VARCHAR(255) NOT NULL,
  Bucket     TINYINT      NOT NULL,   -- 0 didn't like it, 1 it was fine, 2 liked it
  Position   INT          NOT NULL,   -- order within the bucket, 0 = best
  Score      DECIMAL(3,1) NOT NULL,   -- 0.0 to 10.0, computed by the app from bucket and position
  UpdatedAt  DATETIME     NOT NULL,
  UNIQUE KEY UX_Rankings_Device_Place (DeviceId, PlaceId),
  KEY IX_Rankings_Place (PlaceId)
);
