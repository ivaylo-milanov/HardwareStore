import * as request from './api.js';

export async function login(body) {
    let url = '/User/EasyLogin';
    await request.post(url, body);
} 