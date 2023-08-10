import axios from "axios"
import { MemberProps } from "../reducers/MemberReducer"

const url = `${process.env.REACT_APP_API_URL}/member`

export const get = async (props: { signal: AbortSignal }) => {
    let { signal } = props
    let response = await axios.get(url, { signal: signal })
    return response
}

export const remove = async (props: { signal: AbortSignal, id: number }) => {
    const { id, signal } = props
    let response = await axios.delete(`${url}/${id}`, { signal: signal, })
    return response
}

export const paging = async (props: { pageIndex: number, pageSize: number, signal: AbortSignal }) => {
    let { pageIndex, pageSize, signal } = props
    let response = await axios.get(`${url}/paging?PageIndex=${pageIndex}&PageSize=${pageSize}`, { signal: signal })
    return response
}

export const search = async (props: { pageIndex: number, pageSize: number, name: string, signal: AbortSignal }) => {
    let { pageIndex, pageSize, name, signal } = props
    var response = await axios.post(`${url}/search`,
        {
            pageIndex,
            pageSize,
            name
        },
        {
            signal: signal,
        })
    return response
}

export const add = async (props: { signal: AbortSignal, data: MemberProps }) => {
    let { signal, data } = props
    var response = await axios.post(`${url}`, data, { signal: signal, })
    return response
}

export const update = async (props: { signal: AbortSignal, data: MemberProps }) => {
    let { signal, data } = props
    var response = await axios.put(`${url}`, data, { signal: signal, })
    return response
}
