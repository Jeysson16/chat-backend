const fetch = global.fetch;

async function run() {
  const url = process.env.SUPABASE_URL || '';
  const key = process.env.SUPABASE_ANON_KEY || '';
  const userId = process.env.TEST_USER_ID || '1000000001';
  const appCode = process.env.TEST_APP_CODE || 'SICOM_CHAT_2024';
  if (!url || !key) {
    console.error('Faltan SUPABASE_URL y SUPABASE_ANON_KEY');
    process.exit(1);
  }
  const headers = { apikey: key, 'Content-Type': 'application/json', Accept: 'application/json', Prefer: 'return=representation' };
  const uspBody = { cAppCodigo: appCode, cUsuarioId: userId, nPage: 1, nPageSize: 50, perJurCodigo: 'DEFAULT' };
  const uspResp = await fetch(`${url}/rest/v1/rpc/USP_Chat_GetUserConversations`, { method: 'POST', headers, body: JSON.stringify(uspBody) });
  const uspJson = await uspResp.json();
  console.log('USP count:', Array.isArray(uspJson) ? uspJson.length : 0);
  if (!Array.isArray(uspJson) || uspJson.length === 0) {
    const fnBody = { cUsuarioId: userId };
    const fnResp = await fetch(`${url}/rest/v1/rpc/fn_listar_conversaciones_por_usuario`, { method: 'POST', headers, body: JSON.stringify(fnBody) });
    const fnJson = await fnResp.json();
    console.log('FN count:', Array.isArray(fnJson) ? fnJson.length : 0);
    if (!Array.isArray(fnJson) || fnJson.length === 0) {
      console.error('Sin resultados en USP ni FN');
      process.exit(2);
    }
  }
  console.log('OK');
}

run().catch(err => { console.error(err); process.exit(99); });
