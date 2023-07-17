export function getData(name) {
    const data = localStorage.getItem(name);
    return data ? JSON.parse(data) : null;
}

export function setData(name, data) {
    localStorage.setItem(name, JSON.stringify(data));
}

export function removeData(name) {
    localStorage.removeItem(name);
}

export function getInitialURL() {
    return window.location.protocol + '//' + window.location.host;
}