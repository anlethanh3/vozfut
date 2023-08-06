import { useEffect, useReducer } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, Container } from "react-bootstrap";
import { memberReducer, initState } from "../reducers/MemberReducer";
import { paging } from '../providers/MemberApiProvider'
import moment from 'moment'
function Home() {
    const sizes = [10, 20, 30]
    const [state, dispatch] = useReducer(memberReducer, initState)
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const fetchData = async (signal) => {
        try {
            dispatch({ type: 'fetch' })
            var data = await paging(state.pageIndex, state.pageSize, signal)
            dispatch({ type: 'success', payload: data })
        } catch (ex) {
            dispatch({ type: 'failure', error: ex })
        }
    }
    const onChangePageIndex = (pageIndex) => {
        dispatch({ type: 'page', pageIndex: pageIndex })
    }
    const onChangePageSize = (pageSize) => {
        dispatch({ type: 'size', pageSize: pageSize })
    }
    useEffect(() => {
        const abortController = new AbortController();
        fetchData(abortController.signal)
        return () => {
            console.log('unmount', state)
            abortController.abort()
        }
    }, [state.pageIndex, state.pageSize])
    return (
        <>
            <h1>Members</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <Button variant="primary">Add Member</Button><div className="mx-2" />
                    <Button variant="secondary">Import CSV</Button>
                </Col>
            </Row>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Elo</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(element =>
                            <tr key={`key-${element.id}`}>
                                <td>{element.id}</td>
                                <td>{element.name}</td>
                                <td>{element.description}</td>
                                <td>+{element.elo}</td>
                                <td>{element.modifiedDate && moment(element.modifiedDate).format()}</td>
                                <td>
                                    <Button variant="warning">Edit</Button>{' '}
                                    <Button variant="danger">Delete</Button>{' '}
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
            {
                state.totalPage &&
                <>
                    <Row>
                        <Col>
                            <DropdownButton className="d-inline" size="md" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
                                {
                                    sizes.map(element =>
                                        <Dropdown.Item onClick={() => onChangePageSize(element)}>{element}</Dropdown.Item>
                                    )
                                }
                            </DropdownButton> <p className="d-inline">Records Page {state.pageIndex + 1} of {state.totalPage}</p></Col>
                        <Col>
                            <Pagination className="d-flex justify-content-end">
                                {
                                    Array.from(Array(state.totalPage).keys()).map(element => {
                                        var pageNumber = element + 1
                                        return <Pagination.Item
                                            onClick={() => onChangePageIndex(pageNumber - 1)}
                                            key={`pitem-${element}`}
                                            active={state.pageIndex === pageNumber - 1}>{pageNumber}</Pagination.Item>
                                    })
                                }
                            </Pagination>
                        </Col>
                    </Row>
                </>
            }

        </>
    )
}
export default Home;