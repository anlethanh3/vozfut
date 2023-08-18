import axios from "axios"
import { MatchProps } from "../slices/matchSlice"

const url = `${process.env.REACT_APP_API_URL}/match`

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

export const search = async <T>(props: { pageIndex: number, pageSize: number, name: string, signal: AbortSignal }) => {
    let { pageIndex, pageSize, name, signal } = props
    var response = await axios.post<T>(`${url}/search`,
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

export const add = async (props: { signal: AbortSignal, data: MatchProps }) => {
    let { signal, data } = props
    var response = await axios.post(`${url}`, data, { signal: signal, })
    return response
}

export const update = async (props: { signal: AbortSignal, data: MatchProps }) => {
    let { signal, data } = props
    var response = await axios.put(`${url}`, data, { signal: signal, })
    return response
}
