-- Twin Cities metro: shops that answer a searched-for question: 33 shops. Review restore-review.md before running.
START TRANSACTION;
UPDATE CoffeeShops SET isDeleted = 0
WHERE isDeleted = 1 AND placeId IN (
  'ChIJp4rrZWols1IRMv-1KewB8e8',
  'ChIJm9l2Hbojs1IR7Ff_87GkaWo',
  'ChIJvUQPwdAi9ocRxxeBTb4-G1o',
  'ChIJxwp9dgAv9ocRhWS2euYX5Dc',
  'ChIJ7XKyCC479ocRjNTR2Y43DoU',
  'ChIJgQoBp_Y69ocRVHmVnegCXT4',
  'ChIJpxHCQOYvs1IRP11jzmko5jo',
  'ChIJ-7-i8H0t9ocRe7zl4cY26kk',
  'ChIJi393cN0Z9ocRphdOAz52ZBU',
  'ChIJDZSwcUQg9ocREYoto6ktAOQ',
  'ChIJI9q2QZTNslIRNZMxUk5F14o',
  'ChIJAczPUwdHs1IRZoZi83oxlqQ',
  'ChIJTbqiRSon9ocRdmmSVXnf2AU',
  'ChIJtzjivWoo9ocRZi57ad45jXo',
  'ChIJf3gRVrQys1IR0kG3nwSGOIw',
  'ChIJh7zkC3wxs1IRjGLtI6NXuxo',
  'ChIJi5UUpDwzs1IREmfYyt6ObGQ',
  'ChIJT0jD3N4zs1IRutUnrtyEIb8',
  'ChIJCXgYS4kys1IRH8jmbbPj-zY',
  'ChIJf-Ar4UAn9ocR7hNKmlh05o0',
  'ChIJKblc5oQvs1IRKkqVTRcDn9s',
  'ChIJuRIbD4An9ocRUAuWzo70jlg',
  'ChIJUYDhnycp9ocRmV6NayLwrAI',
  'ChIJZSXskBsvs1IRcx9DlzZ6JJg',
  'ChIJg5ZN4981s1IR5c29CkN4DUE',
  'ChIJSZzQrrMrs1IRBFvtIvIZmvc',
  'ChIJXyNZFJQr9ocR4UFQbCX8w6A',
  'ChIJ894-6Ioq9ocRO1KIwauCOzE',
  'ChIJ_4onGLgss1IRwrDvJj4dpCg',
  'ChIJ6yFezLj_VEARh9iSQMPB2bI',
  'ChIJrZ6rQgDRslIR2C0riVdvi_Y',
  'ChIJt1YReLfQslIRZ2-FjodSfMQ',
  'ChIJH5R9yhLRslIRHGguPudoNS8'
);
SELECT ROW_COUNT() AS restored;
COMMIT;
