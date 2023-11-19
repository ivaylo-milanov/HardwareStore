import { getInitialURL } from "./utils.js";

const host = getInitialURL();

export async function request(method, url, body) {
    const options = {
        method,
        headers: {}
    }

    if (body) {
        options.headers['Content-Type'] = 'application/json';
        options.body = body
    }

    try {
        const response = await fetch(host + url, options);

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message);
        }

        if (response.status === 204) {
            return response;
        }

        const data = await response.text();

        return data;
    } catch (err) {
        console.error('An error occured', err);
        throw err;
    }
}

const get = request.bind(null, 'get');
const post = request.bind(null, 'post');
const put = request.bind(null, 'put');
const del = request.bind(null, 'delete');

export {
    get,
    post,
    put,
    del as delete
}