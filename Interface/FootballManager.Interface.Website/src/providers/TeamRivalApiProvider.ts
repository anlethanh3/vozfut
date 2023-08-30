import axios from "axios"
import { MemberProps } from "../reducers/MemberReducer"

const url = `${process.env.REACT_APP_API_URL}/teamrival`

export const get = async <T>(props: { signal: AbortSignal, id: number }) => {
    let { id, signal } = props
    let response = await axios.get<T>(`${url}/${id}`, { signal: signal })
    return response
}