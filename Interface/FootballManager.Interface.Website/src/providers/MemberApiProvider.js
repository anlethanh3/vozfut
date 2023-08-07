var url = "http://localhost:5000/member"

const get = async (signal) => {
    var response = await fetch(url, { signal: signal })
    return response.json()
}

const remove = async (signal, id) => {
    var response = await fetch(`${url}/${id}`, {
        signal: signal,
        method: 'DELETE'
    })
    return response
}

const paging = async (pageIndex, pageSize, signal) => {
    var response = await fetch(`${url}/paging?PageIndex=${pageIndex}&PageSize=${pageSize}`, { signal: signal })
    return response.json()
}

const add = async (signal, member) => {
    var response = await fetch(`${url}`, {
        signal: signal,
        body: JSON.stringify({
            name: member.name,
            elo: member.elo,
            description: member.description,
        }),
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    return response
}

const update = async (signal, member) => {
    var response = await fetch(`${url}`, {
        signal: signal,
        body: JSON.stringify({
            id: member.id,
            name: member.name,
            elo: member.elo,
            description: member.description,
        }),
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    return response
}

export { get, paging, add, remove, update}