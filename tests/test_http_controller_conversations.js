const fetch = global.fetch;

async function run() {
  const base = process.env.CHAT_BASE_URL || 'http://localhost:5406/api';
  const userId = process.env.TEST_USER_ID || '1000000001';
  const appCode = process.env.TEST_APP_CODE || 'SICOM_CHAT_2024';
  const params = new URLSearchParams({
    userId,
    userid: userId,
    nPerId: userId,
    cPerCodigo: userId,
    perJurCodigo: 'DEFAULT',
    cPerJurCodigo: 'DEFAULT',
    appCode: appCode,
    appId: appCode
  });
  const url = `${base}/chat/conversations?${params.toString()}`;
  const resp = await fetch(url, { method: 'GET' });
  const json = await resp.json().catch(() => []);
  const arr = Array.isArray(json) ? json : (Array.isArray(json?.data) ? json.data : (Array.isArray(json?.LstItem) ? json.LstItem : (Array.isArray(json?.lstItem) ? json.lstItem : [])));
  console.log('Controller count:', Array.isArray(arr) ? arr.length : 0);
  console.log('Sample:', Array.isArray(arr) && arr.length > 0 ? arr[0] : null);
  console.log('OK');
}

run().catch(err => { console.error(err); process.exit(99); });
