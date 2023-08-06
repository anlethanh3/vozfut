var url = "http://localhost:5000/member"

const get = async (signal) => {
    var response = await fetch(url, { signal: signal })
    return response.json()
}
const paging = async (pageIndex, pageSize, signal) => {
    var response = await fetch(`${url}/paging?PageIndex=${pageIndex}&PageSize=${pageSize}`, { signal: signal })
    return response.json()
}
export { get, paging }