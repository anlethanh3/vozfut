import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { useEffect, useState } from 'react';
import { MemberProps } from '../reducers/MemberReducer';

const UpdateMember = (props: { show: boolean, initData: MemberProps | undefined, onSubmit: (member: MemberProps) => void, onClose: () => void }) => {
    const { show, initData, onSubmit, onClose } = props
    const [member, setMember] = useState<MemberProps>({ id: 0, name: '', description: '', elo: 1, })
    const [valid, setValid] = useState({ name: true });
    const onValid = () => {
        if (member.name === '') {
            setValid({ ...valid, name: false })
            return
        }
        onSubmit(member)
    }
    useEffect(() => {
        if (initData) {
            setMember({ ...initData })
        }
    }, [initData])

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update a member</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMember.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={!valid.name} placeholder="Input name" value={member.name} onChange={(e) => setMember({ ...member, name: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Name is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Description">
                        <FloatingLabel controlId="floatingDescription" label="Description">
                            <Form.Control type="text" placeholder="Input description" value={member.description} onChange={(e) => setMember({ ...member, description: e.target.value })} />
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Elo">
                        <FloatingLabel controlId="floatingElo" label="Elo">
                            <Form.Select value={member.elo} onChange={(e) => setMember({ ...member, elo: parseInt(e.target.value) })}>
                                {
                                    [1, 2, 3, 4, 5].map(value =>
                                        <option key={`elo.id-${value}`} value={value}>+{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}

export default UpdateMember